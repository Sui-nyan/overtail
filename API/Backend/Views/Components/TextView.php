<?php

class TextView extends View
{
	/**
	 * Creates a text paragraph
	 *
	 * @param string $text Content of the paragraph
	 */
	public function __construct(
		private string $text
	){}

	public function render(): void {
		echo "<p>{$this->text}</p>";
	}
}
