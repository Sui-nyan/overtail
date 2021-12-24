<?php

class SafeInvController extends Controller
{
    protected array $paths = ['/inv/save', '/inventory/save'];
    protected array $methods = ['POST'];
    protected array $reqVar = ['invData'];

    protected function execute(): void {
        $invData = IO::POST('invData');
        $uuid = Auth::tokenUuid();

        // TODO: Delete! Items that are in the DB but not in the $invData
        foreach ($invData as $item) {
            $q = new Query('SELECT COUNT(`uuid`) count FROM Inventory WHERE `uuid`=:uuid AND `slot`=:slot;', [':uuid' => $uuid, ':slot' => $item['slot']]);
            if ($q->fetch()['count'] == 1) {
                $itemData = [
                    ':item' => $item['item'],
                    ':slot' => $item['slot'],
                    ':amount' => $item['amount']
                ];
                (new Query('UPDATE SET `item`=:item, `slot`=:slot, `amount`=:amount WHERE `uuid`=:uuid AND `slot`=:slot;', $itemData));
            } elseif ($q->fetch()['count'] == 0) {
                $itemData = [
                    ':uuid' => $uuid,
                    ':item' => $item['item'],
                    ':slot' => $item['slot'],
                    ':amount' => $item['amount']
                ];
                (new Query('INSERT INTO `Inventory` (`uuid`, `item`, `slot`, `amount`) VALUES (:uuid, :item, :slot, :amount);', $itemData));
            }
        }
    }
}
