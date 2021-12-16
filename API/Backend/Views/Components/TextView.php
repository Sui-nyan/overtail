<?php

class TextView extends View
{
	public function __construct(
		private string $text
	){}

	public function render(): void {
		echo "<p>{$this->text}</p>";
	}
}
