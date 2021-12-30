<?php

class PlayerSafeController extends Controller
{
    protected array $paths = ['/player/save'];
    protected array $methods = ['POST'];
    protected array $reqVar = ['x', 'y'];
    protected bool $userRequired = true;

    protected function execute(): void {
        $save = new Query('INSERT INTO `PlayerData` (uuid, x, y) VALUES (:uuid, :x, :y) ON DUPLICATE KEY UPDATE `x`=:x, `y`=:y;',
            [
                ':uuid' => Auth::tokenUuid(),
                ':x' => IO::POST('x'),
                ':y' => IO::POST('y')
            ]
        );

        (new APIView(['success' => $save->success()]))->render();
    }
}
