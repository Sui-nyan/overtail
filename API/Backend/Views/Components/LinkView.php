<?php

class LinkView extends View
{
	/**
	 * Creates an anchor
	 *
	 * @param string $href Link
     * @param string $title Title
	 */
	public function __construct(
		private string $href,
		private string $title
	){}

	public function render(): void {
		echo "<a href=\"{$this->href}\">{$this->title}</a>";
	}
}
