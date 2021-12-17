<?php

class HomeController extends Controller
{
	protected array $paths = ['/'];

	protected function execute(): void {
		$layout = new LayoutView('API Documentation');

		$cons = array_values(Router::getRoutes());
		foreach ($cons as $con) {
			$layout->addChild(new HeadingView($con::class, 'h2'));
			$layout->addChild(new TextView('User required: ' . $con->getUserReq()));
			$layout->addChild(new TextView('Paths: ' . implode(' ', $con->getPaths())));
			$layout->addChild(new TextView('Methods: ' . implode(', ', $con->getMethods())));
			foreach ($con->getMethods() as $method) {
				if ($method == 'POST') {
					$layout->addChild(new TextView('Variables: ' . implode(', ', $con->getReqVar())));
				}
			}
		}
		$layout->render();
	}
}
