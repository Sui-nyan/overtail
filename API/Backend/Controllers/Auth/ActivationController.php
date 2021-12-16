<?php

class ActivationController extends Controller
{
    protected array $paths = ['/activate/:uuid'];

    protected function execute(): void {
        if (($uuid = $this->getParam('uuid')) != null) {
            $q = new Query('UPDATE `User` SET `activation`=1 WHERE `uuid`=:uuid;', [':uuid' => $uuid]);

            if ($q->count() > 0) {
                $layout = new TextView('Activation successful');
            } else $layout = new TextView('Failed to activate your account');
        } else $layout = new ErrorView(400, false);

        $layout->render();
    }
}
