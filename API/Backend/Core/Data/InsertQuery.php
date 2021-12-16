<?php

class InsertQuery extends Query
{
	public function __construct(string $tableName) {
		$this->queryStr = "INSERT INTO `{$tableName}` VALUES (";
	}

	/**
	 * Adds a value to the insert query
	 *
	 * @param string $columnName Name of the column (has to be same as in DB)
	 * @param string $value Value for the column
	 */
	public function add(string $columnName, string $value): void {
		if ($this->values != null)
			$this->queryStr .= ",";
		else
			$this->values = [];
		$placeholder = ":" . strtolower($columnName);
		$this->queryStr .= " `$columnName`=";
		$this->queryStr .= $placeholder;

		$this->values[$placeholder] = $value;
	}

	/**
	 * Executes the query
	 *
	 * @param string|null $idName Name of the ID field
	 * @return int|string Last insert ID
	 */
	public function run(?string $idName = null): int|string {
		$this->queryStr .= ");";
		return $this->lastInsertId($idName);
	}
}
