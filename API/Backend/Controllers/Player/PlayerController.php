<?php

class PlayerController extends Controller
{
    protected array $paths = ['/player'];
    protected bool $userRequired = true;

    protected function execute(): void {
        $pos = (new Query('SELECT `x`, `y` FROM `PlayerData` WHERE `uuid`=:uuid;', [':uuid' => Auth::tokenUuid()]))->fetch();
        $inv = (new Query('SELECT `item` id, `amount` FROM `Inventory` WHERE `uuid`=:uuid;', [':uuid' => Auth::tokenUuid()]))->fetchAll();

        (new APIView(['position' => $pos, 'inventory' => $inv]))->render();
    }
}
