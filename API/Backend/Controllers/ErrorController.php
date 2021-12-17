<?php

class ErrorController extends Controller
{
	protected function execute(): void {
		(new LayoutView(''))->addChild(new ErrorView(isAPI: false))->render();
	}
}
