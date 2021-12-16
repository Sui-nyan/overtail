<?php

/**
 * Base class of all database models
 */
abstract class Model
{
	/**
	 * @var string Table name (has to be same as in database!)
	 */
	private string $tableName;

	/**
	 * @var int Binds model to database entry
	 */
	private int $id;

	/**
	 * Creates a model
	 *
	 * @param int $id Binds the model object to an ID in the database
	 */
	public function __construct(int $id) {
		$this->tableName = get_class($this);
		$this->id = $id;
	}

	/**
	 * Gets the ID
	 *
	 * @return integer ID of the model
	 */
	public function getID(): int {
		return $this->id;
	}

	/**
	 * Sets a value of a field in the database
	 *
	 * @param string $field Field name in the database
	 * @param string $value New value
	 * @return bool Update successful or not
	 */
	protected function setField(string $field, string $value): bool {
		return (new Query(
			"UPDATE `{$this->tableName}` SET `{$field}`=:value WHERE `ID`=:id;",
			[
				":value" => $value,
				":id" => $this->id
			]
		))->success();
	}

	/**
	 * Gets a value from the database
	 *
	 * @param string $field Field name in the database
	 * @return string|null The value from the database (could be null)
	 */
	protected function getField(string $field): ?string {
		return (new Query(
			"SELECT `{$field}` FROM `{$this->tableName}` WHERE `ID`=:id;",
			[
				":id" => $this->id
			]
		))->fetch()[$field];
	}
}
