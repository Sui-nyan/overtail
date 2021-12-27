<?php

class HomeController extends Controller
{
	protected array $paths = ['/'];

	protected function execute(): void {
		(new LayoutView('API Documentation'))->render();
	}
}
