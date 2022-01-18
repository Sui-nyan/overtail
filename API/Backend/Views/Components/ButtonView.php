<?php

class ButtonView extends View
{
	/**
	 * Creates a heading
	 *
	 * @param string $content Content of the heading
	 * @param bool $disabled Whether disabled or not
	 */
	public function __construct(
		private string $content,
        private bool $disabled = false
	){}

	public function render(): void {
        if (!$this->disabled)
    		echo "<button onclick=\"alert('//TODO: Implement and replace alert! It\'s not 1995 anymore');\" type=\"button\">{$this->content}</button>";
        else
            echo "<button type=\"button\" disabled>{$this->content}</button>";
	}
}
