# Documentation

![Auth token required](.lock-fill.svg): Auth token required to access route

## Routes

### `POST /login`

| Variable     | Required  | Type       |
|--------------|-----------|------------|
| username     | true      | string     |
| password     | true      | string     |

### Response

```json
// Success: uuid, token; Failure: 480 => Wrong password
{
  "uuid": "8eb392d4-b069-43fa-83e4-29e26f0ceede",
  "token": "T0dWaU16a3laRFF0WWpBMk9TMDBNMlpoTFRnelpUUXRNamxsTWpabU1HTmxaV1JsLkpESjVKREV3SkdwSldVRTRkV3BRTUdKdE9VbGlNMUp3Ulhod05uVXhRbVZSTUdSalFrWXpabGRVTms1S1NFbEhhVkZVWVdoMkwzbHdWRFZsLk1qQXlNaTB3TVMwd05nPT0="
}
```

### `POST /register`

| Variable     | Required  | Type       |
|--------------|-----------|------------|
| username     | true      | string     |
| password     | true      | string     |

### Response

```json
{
  "success": true   // False on failure
}
```

### `GET [/inv | /inventory]` ![Auth token required](.lock-fill.svg)

### Response

```json
[
  {
    // Item
  },
  {
    // Item
  }
]
```
