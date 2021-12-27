<?php

class InventoryController extends Controller
{
    protected array $paths = ['/inv', '/inventory'];
    protected array $methods = ['GET', 'POST'];
    protected bool $userRequired = true;

    protected function execute(): void {
        $q = new Query('SELECT `item` id, `amount`, `slot` FROM `Inventory` WHERE `uuid`=:uuid;', [':uuid' => Auth::tokenUuid()]);
        (new APIView($q->fetchAll()))->render();
    }
}
