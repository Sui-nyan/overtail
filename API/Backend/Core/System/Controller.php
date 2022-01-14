<?php

/**
 * Base class of all the controllers handling the routes
 */
abstract class Controller
{
	/**
	 * @var array<string,string> param name, value
	 */
	private array $params;

	/**
	 * @var string[] Paths the controller listens to
	 */
	protected array $paths = [];

	/**
	 * @var string[] Allowed methods to access controller
	 */
	protected array $methods = ['GET'];

	/**
	 * @var string[] Required POST variables
	 */
	protected array $reqVar = [];

	/**
	 * Defines whether user authentication is required
	 */
	protected bool $userRequired = false;

	/**
	 * Adds the paths to the router
	 */
	public function initRoutes(): void {
		foreach ($this->paths as $path)
			Router::addRoute($path, $this);
	}

	/**
	 * Executes the controller with given params
	 *
	 * @param array<string,string> $params Parameters
	 */
	public function runExecute(array $params): void {
		if (($code = $this->checkAccess()) === 200) {
			$this->params = $params;
			$this->execute();
		} else {
			(new ErrorView($code))->render();
		}
	}

	/**
	 * Main method of the controller
	 */
	abstract protected function execute(): void;

	/**
	 * Get a param from within the URL
	 *
	 * @param string $var Name of variable
	 * @param boolean $exact Get the exact value
	 * @return string|null Value of variable or null if not exists
	 */
	protected function getParam(string $var, bool $exact = false): ?string {
		if (isset($this->params[$var]))
			if (is_string($this->params[$var]))
				return $exact ? strval($this->params[$var]): htmlspecialchars(urldecode(strval($this->params[$var])));
		return null;
	}

	/**
	 * Redirects user to new URL
	 *
	 * @param string $url Redirectes to this
	 */
	protected function redirect(string $url): never {
		header('Location: ' . $url);
		exit;
	}

	/**
	 * Checks if the access to this controller is allowed
	 *
	 * @return int HTTP status code (200 === OK!)
	 */
	private function checkAccess(): int {
		if (empty(array_intersect(['*', 'OPTIONS', 'HEAD', IO::method()], $this->methods)))
			return 405;
		if ($this->userRequired)
			if (!Auth::validateToken())
				return 401;
		foreach ($this->reqVar as $var)
			if (IO::POST($var) === null)
				return 400;
		return 200;
	}
}
