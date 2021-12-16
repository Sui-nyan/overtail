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
	<title><?=$this->title?></title>
	<base href="/">

	<meta name="color-scheme" content="dark light">
	<meta name="theme-color" media="(prefers-color-scheme: light)" content="#EEEEEE">
	<meta name="theme-color" media="(prefers-color-scheme: dark)" content="#101010">

	<link rel="stylesheet" href="/Frontend/css/main.css">
</head>
<body>
	<header>
		<a href="/">Home</a>
	</header>

	<main>
		<h1><?=$this->title?></h1>
		<?php $this->renderChildren(); ?>
	</main>

	<footer>
		&copy <?=date('Y')?> &middot; Felix Schindler
	</footer>
</body>
</html>
<?php
    }
}
