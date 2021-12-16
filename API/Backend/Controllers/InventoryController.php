<?php

class InventoryController extends Controller
{
    protected array $paths = ['/inv', '/inventory'];
    protected array $methods = ['GET', 'POST'];
    protected bool $userRequired = true;

    protected function execute(): void {
        // Insert data into the inventory db
        if (($invData = IO::POST('invData')) !== null) {
            $invData = json_decode($invData);
            foreach ($invData as $item) {
                $q = new Query('SELECT * FROM `Inventory` ');
                if ($q->count() > 0) {
                    // TODO: Update amount, ...
                }

                else {
                    // TODO: Insert item
                }
            }
            // TODO: Implement
        }

        // Show user inventory
        else {
            $q = new Query('SELECT * FROM `Inventory` WHERE `uuid`=:uuid;', [':uuid' => Auth::tokenUuid()]);
            (new APIView($q->fetchAll()))->render();
        }
    }
}
