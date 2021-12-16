<?php

class APIView extends View
{
	/**
	 * @param array<mixed>|null $data JSON data to be displayed
	 */
	public function __construct(
		public ?array $data = null
	){}

	public function render(): void {
		header("Content-type: application/json; charset=utf-8");
		// header("Access-Control-Allow-Origin: " . DOMAIN);
		// header("Access-Control-Allow-Headers: Authorization, Origin, X-Requested-With, Content-Type, Accept");
		// header("Access-Control-Max-Age: 86400");

		echo json_encode($this->data);
	}
}
