<?php

class Teapot extends Controller
{
	protected array $paths = ['/teapot'];

	protected function execute(): void {
		(new ErrorView(418, false))->render();
	}
}
