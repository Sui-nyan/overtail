<?php

class HeadingView extends View
{
	public function __construct(
		private string $title,
		private string $size = "h1"
	){}

	public function render(): void {
		echo "<{$this->size}>{$this->title}</{$this->size}>";
	}
}
