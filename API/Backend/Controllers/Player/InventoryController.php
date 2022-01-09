<?php

class InventoryController extends Controller
{
    protected array $paths = ['/inv', '/inventory'];
    protected bool $userRequired = true;

    protected function execute(): void {
        $inv = (new Query('SELECT `item` id, `amount` FROM `Inventory` WHERE `uuid`=:uuid;', [':uuid' => Auth::tokenUuid()]))->fetchAll();
        (new APIView($inv))->render();
    }
}
