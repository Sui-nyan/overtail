<?php

class LayoutView extends View
{
	function __construct(
		private string $title
	){}

	public function render(): void {
?>
<!DOCTYPE html>
<html lang="de">
<head>
	<meta charset="UTF-8">
	<meta name="viewport" content="width=device-width, initial-scale=1.0">
	<title><?=TITLE . " &middot; " . $this->title?></title>
	<base href="/">

	<meta name="color-scheme" content="dark light">
    <meta name="theme-color" media="(prefers-color-scheme: light)" content="white">
    <meta name="theme-color" media="(prefers-color-scheme: dark)" content="#1f2348">

	<link rel="icon" href="/animated_favicon.gif" type="image/gif">

	<link rel="stylesheet" href="https://felix-schindler.github.io/nimiq-style/nimiq-style.min.css">
	<link rel="stylesheet" href="/Frontend/css/main.css">
</head>
<body>
	<header>
		<a href="/">Home</a>
		<a href="https://gitlab.mi.hdm-stuttgart.de/mb365/Overtail">GitLab</a>
	</header>

	<main>
		<h1><?=$this->title?></h1>
		<?php $this->renderChildren(); ?>
	</main>

	<footer>
		<p>&copy; 2021-<?=date('Y')?> &middot; <a href="https://schindlerfelix.de">Felix Schindler</a></p>
	</footer>
</body>
</html>
<?php
    }
}
