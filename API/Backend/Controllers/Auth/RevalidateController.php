<?php

class RevalidateController extends Controller
{
    protected array $paths = ['/revalidate'];
    protected array $methods = ['POST'];
    protected array $reqVar = ['password'];

    protected function execute(): void {
        $uuid = Auth::tokenUuid();
        $q = new Query('SELECT `uuid`, `password` FROM `User` WHERE `email`=:email AND `activation` IS NULL;', [':uuid' => $uuid]);

        if (($user = $q->fetch()) !== null) {
            if (password_verify(IO::POST('password'), $user['password'])) {
                (new APIView([
                    'token' => Auth::generateToken($uuid, $user['password'])
                ]))->render();
                return;
            } else $error = 480;
        } else $error = 482;

        (new ErrorView($error))->render();
    }
}
