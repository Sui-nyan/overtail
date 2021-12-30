<?php

class PlayerController extends Controller
{
    protected array $paths = ['/player'];
    protected bool $userRequired = true;

    protected function execute(): void {
        $data = (new Query('SELECT `x`, `y` FROM `PlayerData` WHERE `uuid`=:uuid;', [':uuid' => Auth::tokenUuid()]))->fetch();
        (new APIView($data))->render();
    }
}
