// <auto-generated>
//     Generated by the protocol buffer compiler.  DO NOT EDIT!
//     source: kafka_proto_message.proto
// </auto-generated>
#pragma warning disable 1591, 0612, 3021, 8981
#region Designer generated code

using pb = global::Google.Protobuf;
using pbc = global::Google.Protobuf.Collections;
using pbr = global::Google.Protobuf.Reflection;
using scg = global::System.Collections.Generic;
/// <summary>Holder for reflection information generated from kafka_proto_message.proto</summary>
public static partial class KafkaProtoMessageReflection {

  #region Descriptor
  /// <summary>File descriptor for kafka_proto_message.proto</summary>
  public static pbr::FileDescriptor Descriptor {
    get { return descriptor; }
  }
  private static pbr::FileDescriptor descriptor;

  static KafkaProtoMessageReflection() {
    byte[] descriptorData = global::System.Convert.FromBase64String(
        string.Concat(
          "ChlrYWZrYV9wcm90b19tZXNzYWdlLnByb3RvIkgKEUthZmthUHJvdG9NZXNz",
          "YWdlEhAKCFR5cGVOYW1lGAEgASgJEhMKC1R5cGVWZXJzaW9uGAIgASgJEgwK",
          "BERhdGEYAyABKAliBnByb3RvMw=="));
    descriptor = pbr::FileDescriptor.FromGeneratedCode(descriptorData,
        new pbr::FileDescriptor[] { },
        new pbr::GeneratedClrTypeInfo(null, null, new pbr::GeneratedClrTypeInfo[] {
          new pbr::GeneratedClrTypeInfo(typeof(global::KafkaProtoMessage), global::KafkaProtoMessage.Parser, new[]{ "TypeName", "TypeVersion", "Data" }, null, null, null, null)
        }));
  }
  #endregion

}
#region Messages
public sealed partial class KafkaProtoMessage : pb::IMessage<KafkaProtoMessage>
#if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
    , pb::IBufferMessage
#endif
{
  private static readonly pb::MessageParser<KafkaProtoMessage> _parser = new pb::MessageParser<KafkaProtoMessage>(() => new KafkaProtoMessage());
  private pb::UnknownFieldSet _unknownFields;
  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
  public static pb::MessageParser<KafkaProtoMessage> Parser { get { return _parser; } }

  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
  public static pbr::MessageDescriptor Descriptor {
    get { return global::KafkaProtoMessageReflection.Descriptor.MessageTypes[0]; }
  }

  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
  pbr::MessageDescriptor pb::IMessage.Descriptor {
    get { return Descriptor; }
  }

  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
  public KafkaProtoMessage() {
    OnConstruction();
  }

  partial void OnConstruction();

  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
  public KafkaProtoMessage(KafkaProtoMessage other) : this() {
    typeName_ = other.typeName_;
    typeVersion_ = other.typeVersion_;
    data_ = other.data_;
    _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
  }

  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
  public KafkaProtoMessage Clone() {
    return new KafkaProtoMessage(this);
  }

  /// <summary>Field number for the "TypeName" field.</summary>
  public const int TypeNameFieldNumber = 1;
  private string typeName_ = "";
  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
  public string TypeName {
    get { return typeName_; }
    set {
      typeName_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
    }
  }

  /// <summary>Field number for the "TypeVersion" field.</summary>
  public const int TypeVersionFieldNumber = 2;
  private string typeVersion_ = "";
  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
  public string TypeVersion {
    get { return typeVersion_; }
    set {
      typeVersion_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
    }
  }

  /// <summary>Field number for the "Data" field.</summary>
  public const int DataFieldNumber = 3;
  private string data_ = "";
  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
  public string Data {
    get { return data_; }
    set {
      data_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
    }
  }

  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
  public override bool Equals(object other) {
    return Equals(other as KafkaProtoMessage);
  }

  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
  public bool Equals(KafkaProtoMessage other) {
    if (ReferenceEquals(other, null)) {
      return false;
    }
    if (ReferenceEquals(other, this)) {
      return true;
    }
    if (TypeName != other.TypeName) return false;
    if (TypeVersion != other.TypeVersion) return false;
    if (Data != other.Data) return false;
    return Equals(_unknownFields, other._unknownFields);
  }

  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
  public override int GetHashCode() {
    int hash = 1;
    if (TypeName.Length != 0) hash ^= TypeName.GetHashCode();
    if (TypeVersion.Length != 0) hash ^= TypeVersion.GetHashCode();
    if (Data.Length != 0) hash ^= Data.GetHashCode();
    if (_unknownFields != null) {
      hash ^= _unknownFields.GetHashCode();
    }
    return hash;
  }

  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
  public override string ToString() {
    return pb::JsonFormatter.ToDiagnosticString(this);
  }

  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
  public void WriteTo(pb::CodedOutputStream output) {
  #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
    output.WriteRawMessage(this);
  #else
    if (TypeName.Length != 0) {
      output.WriteRawTag(10);
      output.WriteString(TypeName);
    }
    if (TypeVersion.Length != 0) {
      output.WriteRawTag(18);
      output.WriteString(TypeVersion);
    }
    if (Data.Length != 0) {
      output.WriteRawTag(26);
      output.WriteString(Data);
    }
    if (_unknownFields != null) {
      _unknownFields.WriteTo(output);
    }
  #endif
  }

  #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
  void pb::IBufferMessage.InternalWriteTo(ref pb::WriteContext output) {
    if (TypeName.Length != 0) {
      output.WriteRawTag(10);
      output.WriteString(TypeName);
    }
    if (TypeVersion.Length != 0) {
      output.WriteRawTag(18);
      output.WriteString(TypeVersion);
    }
    if (Data.Length != 0) {
      output.WriteRawTag(26);
      output.WriteString(Data);
    }
    if (_unknownFields != null) {
      _unknownFields.WriteTo(ref output);
    }
  }
  #endif

  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
  public int CalculateSize() {
    int size = 0;
    if (TypeName.Length != 0) {
      size += 1 + pb::CodedOutputStream.ComputeStringSize(TypeName);
    }
    if (TypeVersion.Length != 0) {
      size += 1 + pb::CodedOutputStream.ComputeStringSize(TypeVersion);
    }
    if (Data.Length != 0) {
      size += 1 + pb::CodedOutputStream.ComputeStringSize(Data);
    }
    if (_unknownFields != null) {
      size += _unknownFields.CalculateSize();
    }
    return size;
  }

  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
  public void MergeFrom(KafkaProtoMessage other) {
    if (other == null) {
      return;
    }
    if (other.TypeName.Length != 0) {
      TypeName = other.TypeName;
    }
    if (other.TypeVersion.Length != 0) {
      TypeVersion = other.TypeVersion;
    }
    if (other.Data.Length != 0) {
      Data = other.Data;
    }
    _unknownFields = pb::UnknownFieldSet.MergeFrom(_unknownFields, other._unknownFields);
  }

  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
  public void MergeFrom(pb::CodedInputStream input) {
  #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
    input.ReadRawMessage(this);
  #else
    uint tag;
    while ((tag = input.ReadTag()) != 0) {
      switch(tag) {
        default:
          _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
          break;
        case 10: {
          TypeName = input.ReadString();
          break;
        }
        case 18: {
          TypeVersion = input.ReadString();
          break;
        }
        case 26: {
          Data = input.ReadString();
          break;
        }
      }
    }
  #endif
  }

  #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
  void pb::IBufferMessage.InternalMergeFrom(ref pb::ParseContext input) {
    uint tag;
    while ((tag = input.ReadTag()) != 0) {
      switch(tag) {
        default:
          _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, ref input);
          break;
        case 10: {
          TypeName = input.ReadString();
          break;
        }
        case 18: {
          TypeVersion = input.ReadString();
          break;
        }
        case 26: {
          Data = input.ReadString();
          break;
        }
      }
    }
  }
  #endif

}

#endregion


#endregion Designer generated code
