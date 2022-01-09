<?php

class HeadingView extends View
{
	/**
	 * Creates a heading
	 *
	 * @param string $title Content of the heading
	 * @param string $size HTML-Tag name h1 - h6
	 */
	public function __construct(
		private string $title,
		private string $size = "h1"
	){}

	public function render(): void {
		echo "<{$this->size}>{$this->title}</{$this->size}>";
	}
}
