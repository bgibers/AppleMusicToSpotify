using System;

using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace SpotifyEnhancer.DataAccess.Models
{

    public partial class Library
    {
        [JsonProperty("data")]
        public Data[] Data { get; set; }

        [JsonProperty("href")]
        public string Href { get; set; }

        [JsonProperty("next")]
        public string Next { get; set; }
    }

    public partial class Data
    {
        [JsonProperty("attributes")]
        public Attributes Attributes { get; set; }

        [JsonProperty("href")]
        public string Href { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("type")]
        public TypeEnum Type { get; set; }
    }

    public partial class Attributes
    {
        [JsonProperty("albumName")]
        public string AlbumName { get; set; }

        [JsonProperty("artistName")]
        public ArtistName ArtistName { get; set; }

        [JsonProperty("artwork")]
        public Artwork Artwork { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("playParams")]
        public PlayParams PlayParams { get; set; }

        [JsonProperty("trackNumber")]
        public long TrackNumber { get; set; }

        [JsonProperty("contentRating", NullValueHandling = NullValueHandling.Ignore)]
        public string ContentRating { get; set; }
    }

    public partial class Artwork
    {
        [JsonProperty("height")]
        public long Height { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("width")]
        public long Width { get; set; }
    }

    public partial class PlayParams
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("isLibrary")]
        public bool IsLibrary { get; set; }

        [JsonProperty("kind")]
        public Kind Kind { get; set; }
    }

    public enum ArtistName { Jungle, LedZeppelin, PinkFloyd, Queen, QueenWyclefJean, Sublime, U2, WienerMozartEnsembleWilliBoskovsky };

    public enum Kind { Song };

    public enum TypeEnum { LibrarySongs };
    
    internal static class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters = {
                ArtistNameConverter.Singleton,
                KindConverter.Singleton,
                TypeEnumConverter.Singleton,
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
        };
    }

    internal class ArtistNameConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(ArtistName) || t == typeof(ArtistName?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            switch (value)
            {
                case "Jungle":
                    return ArtistName.Jungle;
                case "Led Zeppelin":
                    return ArtistName.LedZeppelin;
                case "Pink Floyd":
                    return ArtistName.PinkFloyd;
                case "Queen":
                    return ArtistName.Queen;
                case "Queen & Wyclef Jean":
                    return ArtistName.QueenWyclefJean;
                case "Sublime":
                    return ArtistName.Sublime;
                case "U2":
                    return ArtistName.U2;
                case "Wiener Mozart Ensemble & Willi Boskovsky":
                    return ArtistName.WienerMozartEnsembleWilliBoskovsky;
            }
            throw new Exception("Cannot unmarshal type ArtistName");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (ArtistName)untypedValue;
            switch (value)
            {
                case ArtistName.Jungle:
                    serializer.Serialize(writer, "Jungle");
                    return;
                case ArtistName.LedZeppelin:
                    serializer.Serialize(writer, "Led Zeppelin");
                    return;
                case ArtistName.PinkFloyd:
                    serializer.Serialize(writer, "Pink Floyd");
                    return;
                case ArtistName.Queen:
                    serializer.Serialize(writer, "Queen");
                    return;
                case ArtistName.QueenWyclefJean:
                    serializer.Serialize(writer, "Queen & Wyclef Jean");
                    return;
                case ArtistName.Sublime:
                    serializer.Serialize(writer, "Sublime");
                    return;
                case ArtistName.U2:
                    serializer.Serialize(writer, "U2");
                    return;
                case ArtistName.WienerMozartEnsembleWilliBoskovsky:
                    serializer.Serialize(writer, "Wiener Mozart Ensemble & Willi Boskovsky");
                    return;
            }
            throw new Exception("Cannot marshal type ArtistName");
        }

        public static readonly ArtistNameConverter Singleton = new ArtistNameConverter();
    }

    internal class KindConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(Kind) || t == typeof(Kind?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            if (value == "song")
            {
                return Kind.Song;
            }
            throw new Exception("Cannot unmarshal type Kind");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (Kind)untypedValue;
            if (value == Kind.Song)
            {
                serializer.Serialize(writer, "song");
                return;
            }
            throw new Exception("Cannot marshal type Kind");
        }

        public static readonly KindConverter Singleton = new KindConverter();
    }

    internal class TypeEnumConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(TypeEnum) || t == typeof(TypeEnum?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            if (value == "library-songs")
            {
                return TypeEnum.LibrarySongs;
            }
            throw new Exception("Cannot unmarshal type TypeEnum");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (TypeEnum)untypedValue;
            if (value == TypeEnum.LibrarySongs)
            {
                serializer.Serialize(writer, "library-songs");
                return;
            }
            throw new Exception("Cannot marshal type TypeEnum");
        }

        public static readonly TypeEnumConverter Singleton = new TypeEnumConverter();
    }
}