<?php

class ImageView extends View
{
	/**
	 * Creates an image
	 *
	 * @param string $src Link to image
     * @param string $alt Alt text
	 */
	public function __construct(
		private string $src,
		private string $alt
	){}

	public function render(): void {
		echo "<img src=\"{$this->src}\" alt=\"{$this->alt}\">";
	}
}
