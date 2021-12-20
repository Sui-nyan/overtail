<?php

class InventoryController extends Controller
{
    protected array $paths = ['/inv', '/inventory'];
    protected array $methods = ['GET', 'POST'];
    protected bool $userRequired = true;

    protected function execute(): void {
        // Show user inventory
        if (IO::method() == 'GET') {
            $q = new Query('SELECT `item`, `amount`, `slot` FROM `Inventory` WHERE `uuid`=:uuid;', [':uuid' => Auth::tokenUuid()]);
            $items = [];
            if (($data = $q->fetchAll()) != null) {     // Not null and not empty
                foreach ($data as $item) {
                    $items[] = [
                        'id' => $item['item'],
                        'amount' => intval($item['amount']),
                        'slot' => intval($item['slot']),
                    ];
                }
            }
            (new APIView($items))->render();
        }

        // Insert data into the inventory db
        elseif (IO::method() == 'POST') {
            // TODO: Get and insert items
        }
    }
}
