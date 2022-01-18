<?php

class HomeController extends Controller
{
    protected array $paths = ['/'];

    protected function execute(): void {
        $layout = new LayoutView("Home");

        $layout->addChild(new ButtonView("Download"));
        $layout->addChild(new ImageView("https://cdn.discordapp.com/attachments/785228557240500304/925340639451840512/unknown.png", "In-Game screenshot"));
        $layout->render();
    }
}
