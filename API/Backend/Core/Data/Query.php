<?php

class Query
{
	/**
	 * @var PDO Connection to database
	 */
	private PDO $con;

	/**
	 * @var string Query string
	 */
	protected string $queryStr;

	/**
	 * @var null|array<string,string|float> Array with keys (=placeholder) and values
	 */
	protected ?array $values;

	/**
	 * @var PDOStatement Query object
	 */
	private PDOStatement $stmt;

	/**
	 * @var bool Success
	 */
	private bool $success;

	/**
	 * @var bool Whether query was executed (run) or not
	 */
	private bool $run = false;

	/**
	 * Creates a query with a given string and values.
	 * You SHOULD DEFINETELY use placeholders and an array with the values for execution
	 *
	 * @param string $queryStr Query as a string
	 * @param array<string|int,string|float>|null $values Values for placeholders
	 */
	public function __construct($queryStr = "", ?array $values = null) {
		$this->queryStr = $queryStr;
		$this->values = $values;
		$this->con = new PDO('mysql:host='.DB_HOST.'; dbname='.DB_NAME, DB_USER, DB_PASS);
	}

	/**
	 * Sets a new query string
	 *
	 * @param string $queryStr
	 */
	public function setQueryString(string $queryStr): void {
		$this->queryStr = $queryStr;
	}

	/**
	 * Returns the current query string
	 *
	 * @return string Current query
	 */
	public function getQueryString(): string {
		return $this->queryStr;
	}

	/**
	 * Writes data to database
	 *
	 * @param string|null $name ID row name, must be specified if row name doesn't equal "ID"
	 * @return int|string Last insert ID
	 */
	public function lastInsertId(string $name = null): int|string {
		if (!$this->run)
			$this->execute();
		if ($this->success) {
			$id = $this->con->lastInsertId($name);
			if ($id !== false) {
				if (is_numeric($id))
					return intval($id);
				else
					return $id;
			}
		}
		return 0;
	}

	/**
	 * Reads data (row by row) from database
	 * Access returned value via $return["ColumnName"] or via model class
	 *
	 * @param Model|null $model Model class to be fetched into
	 * @return mixed Given model or result as array - null if query failed
	 */
	public function fetch(Model $model = null): mixed {
		if (!$this->run)
			$this->execute();
		if ($this->success) {
			if ($model !== null)	$this->stmt->setFetchMode(PDO::FETCH_INTO, $model);	// Fetch into a given model class
			else					$this->stmt->setFetchMode(PDO::FETCH_ASSOC);		// Fetch into an array
			if (($result = $this->stmt->fetch()) !== false)
				return $result;															// Return result (when query was successful)
		}
		return null;																	// Fetch or query failed
	}

	/**
	 * Reads data (all rows) from database
	 *
	 * @see https://bugs.php.net/bug.php?edit=2&id=44341
	 * @return array<array<int|string,string>>|null Null on error, array with values (as string!) otherwise (PDO::FETCH_ASSOC)
	 */
	public function fetchAll(): ?array {
		if (!$this->run)
			$this->execute();
		if ($this->success)
			if (($result = $this->stmt->fetchAll(PDO::FETCH_ASSOC)) !== false)
				return $result;
		return null;
	}

	/**
	 * Executes the query and sets success variable
	 */
	private function execute(): void {
		$this->run = true;
		if (($stmt = $this->con->prepare($this->queryStr)) !== false) {
			$this->stmt = $stmt;
			$this->success = $this->stmt->execute($this->values);
		}
	}

	/**
	 * Counts the effected rows by the last query and returns them
	 *
	 * @return integer Effected rows
	 */
	public function count(): int {
		if (!$this->run)
			$this->execute();
		return $this->stmt->rowCount();
	}

	/**
	 * Returns whether the query was successful
	 *
	 * @return boolean Query successful
	 */
	public function success(): bool {
		if (!$this->run)
			$this->execute();
		return $this->success;
	}
}
