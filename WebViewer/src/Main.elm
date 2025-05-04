port module Main exposing (main)

import Browser
import Html exposing (Html, div, text)
import Html.Attributes exposing (class)
import Json.Decode
import Json.Decode.Pipeline
import Json.Encode


port receiveLogs : (Json.Decode.Value -> msg) -> Sub msg


port requestLogs : () -> Cmd msg


type alias LogEntry =
    { tags : List String
    , message : String
    , data : Json.Encode.Value
    }


type alias Model =
    List LogEntry


logDecoder : Json.Decode.Decoder LogEntry
logDecoder =
    Json.Decode.succeed LogEntry
        |> Json.Decode.Pipeline.required "tags" (Json.Decode.list Json.Decode.string)
        |> Json.Decode.Pipeline.required "message" Json.Decode.string
        |> Json.Decode.Pipeline.required "data" Json.Decode.value


decoder : Json.Decode.Decoder (List LogEntry)
decoder =
    Json.Decode.list logDecoder


init : () -> ( Model, Cmd Msg )
init _ =
    ( [], requestLogs () )


type Msg
    = LogUpdate Json.Decode.Value


update : Msg -> Model -> ( Model, Cmd Msg )
update msg _ =
    case msg of
        LogUpdate json ->
            let
                entries =
                    case Json.Decode.decodeValue decoder json of
                        Ok success ->
                            success

                        Err err ->
                            [ { tags = [ "LogUpdate FAILED" ], message = Json.Decode.errorToString err, data = Json.Encode.object [] } ]
            in
            ( entries, Cmd.none )


view : List LogEntry -> Html msg
view model =
    case model of
        [] ->
            div [] [ text "Waiting for logs..." ]

        logs ->
            logs
                |> List.map
                    (\entry ->
                        div [ class "log-entry" ]
                            [ div [ class "tags" ] [ text <| String.join ", " entry.tags ]
                            , div [ class "message" ] [ text entry.message ]
                            , div [ class "data" ] [ text <| Json.Encode.encode 0 entry.data ]
                            ]
                    )
                |> div []


main : Program () (List LogEntry) Msg
main =
    Browser.element
        { init = init
        , update = update
        , subscriptions = \_ -> receiveLogs (\entries -> LogUpdate entries)
        , view = view
        }
