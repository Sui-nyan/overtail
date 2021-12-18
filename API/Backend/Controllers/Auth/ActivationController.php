<?php

class ActivationController extends Controller
{
    protected array $paths = ['/activate/:acToken'];

    protected function execute(): void {
        if (($token = $this->getParam('acToken')) != null) {
            $q = new Query('UPDATE `User` SET `activation`=NULL WHERE `activation`=:act;', [':act' => $token]);

            if ($q->count() > 0) {
                $layout = new TextView('Activation successful');
            } else $layout = new TextView('Failed to activate your account');
        } else $layout = new ErrorView(400, false);

        $layout->render();
    }
}
