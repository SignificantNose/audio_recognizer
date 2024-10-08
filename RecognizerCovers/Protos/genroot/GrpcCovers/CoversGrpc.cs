// <auto-generated>
//     Generated by the protocol buffer compiler.  DO NOT EDIT!
//     source: covers.proto
// </auto-generated>
#pragma warning disable 0414, 1591, 8981, 0612
#region Designer generated code

using grpc = global::Grpc.Core;

namespace GrpcCovers {
  public static partial class CoverMeta
  {
    static readonly string __ServiceName = "GrpcCovers.CoverMeta";

    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static void __Helper_SerializeMessage(global::Google.Protobuf.IMessage message, grpc::SerializationContext context)
    {
      #if !GRPC_DISABLE_PROTOBUF_BUFFER_SERIALIZATION
      if (message is global::Google.Protobuf.IBufferMessage)
      {
        context.SetPayloadLength(message.CalculateSize());
        global::Google.Protobuf.MessageExtensions.WriteTo(message, context.GetBufferWriter());
        context.Complete();
        return;
      }
      #endif
      context.Complete(global::Google.Protobuf.MessageExtensions.ToByteArray(message));
    }

    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static class __Helper_MessageCache<T>
    {
      public static readonly bool IsBufferMessage = global::System.Reflection.IntrospectionExtensions.GetTypeInfo(typeof(global::Google.Protobuf.IBufferMessage)).IsAssignableFrom(typeof(T));
    }

    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static T __Helper_DeserializeMessage<T>(grpc::DeserializationContext context, global::Google.Protobuf.MessageParser<T> parser) where T : global::Google.Protobuf.IMessage<T>
    {
      #if !GRPC_DISABLE_PROTOBUF_BUFFER_SERIALIZATION
      if (__Helper_MessageCache<T>.IsBufferMessage)
      {
        return parser.ParseFrom(context.PayloadAsReadOnlySequence());
      }
      #endif
      return parser.ParseFrom(context.PayloadAsNewBuffer());
    }

    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Marshaller<global::GrpcCovers.AddCoverMetaRequest> __Marshaller_GrpcCovers_AddCoverMetaRequest = grpc::Marshallers.Create(__Helper_SerializeMessage, context => __Helper_DeserializeMessage(context, global::GrpcCovers.AddCoverMetaRequest.Parser));
    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Marshaller<global::GrpcCovers.AddCoverMetaResponse> __Marshaller_GrpcCovers_AddCoverMetaResponse = grpc::Marshallers.Create(__Helper_SerializeMessage, context => __Helper_DeserializeMessage(context, global::GrpcCovers.AddCoverMetaResponse.Parser));
    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Marshaller<global::GrpcCovers.ReadCoverMetaRequest> __Marshaller_GrpcCovers_ReadCoverMetaRequest = grpc::Marshallers.Create(__Helper_SerializeMessage, context => __Helper_DeserializeMessage(context, global::GrpcCovers.ReadCoverMetaRequest.Parser));
    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Marshaller<global::GrpcCovers.ReadCoverMetaResponse> __Marshaller_GrpcCovers_ReadCoverMetaResponse = grpc::Marshallers.Create(__Helper_SerializeMessage, context => __Helper_DeserializeMessage(context, global::GrpcCovers.ReadCoverMetaResponse.Parser));

    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Method<global::GrpcCovers.AddCoverMetaRequest, global::GrpcCovers.AddCoverMetaResponse> __Method_AddCoverMeta = new grpc::Method<global::GrpcCovers.AddCoverMetaRequest, global::GrpcCovers.AddCoverMetaResponse>(
        grpc::MethodType.Unary,
        __ServiceName,
        "AddCoverMeta",
        __Marshaller_GrpcCovers_AddCoverMetaRequest,
        __Marshaller_GrpcCovers_AddCoverMetaResponse);

    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Method<global::GrpcCovers.ReadCoverMetaRequest, global::GrpcCovers.ReadCoverMetaResponse> __Method_ReadCoverMeta = new grpc::Method<global::GrpcCovers.ReadCoverMetaRequest, global::GrpcCovers.ReadCoverMetaResponse>(
        grpc::MethodType.Unary,
        __ServiceName,
        "ReadCoverMeta",
        __Marshaller_GrpcCovers_ReadCoverMetaRequest,
        __Marshaller_GrpcCovers_ReadCoverMetaResponse);

    /// <summary>Service descriptor</summary>
    public static global::Google.Protobuf.Reflection.ServiceDescriptor Descriptor
    {
      get { return global::GrpcCovers.CoversReflection.Descriptor.Services[0]; }
    }

    /// <summary>Base class for server-side implementations of CoverMeta</summary>
    [grpc::BindServiceMethod(typeof(CoverMeta), "BindService")]
    public abstract partial class CoverMetaBase
    {
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual global::System.Threading.Tasks.Task<global::GrpcCovers.AddCoverMetaResponse> AddCoverMeta(global::GrpcCovers.AddCoverMetaRequest request, grpc::ServerCallContext context)
      {
        throw new grpc::RpcException(new grpc::Status(grpc::StatusCode.Unimplemented, ""));
      }

      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual global::System.Threading.Tasks.Task<global::GrpcCovers.ReadCoverMetaResponse> ReadCoverMeta(global::GrpcCovers.ReadCoverMetaRequest request, grpc::ServerCallContext context)
      {
        throw new grpc::RpcException(new grpc::Status(grpc::StatusCode.Unimplemented, ""));
      }

    }

    /// <summary>Client for CoverMeta</summary>
    public partial class CoverMetaClient : grpc::ClientBase<CoverMetaClient>
    {
      /// <summary>Creates a new client for CoverMeta</summary>
      /// <param name="channel">The channel to use to make remote calls.</param>
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public CoverMetaClient(grpc::ChannelBase channel) : base(channel)
      {
      }
      /// <summary>Creates a new client for CoverMeta that uses a custom <c>CallInvoker</c>.</summary>
      /// <param name="callInvoker">The callInvoker to use to make remote calls.</param>
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public CoverMetaClient(grpc::CallInvoker callInvoker) : base(callInvoker)
      {
      }
      /// <summary>Protected parameterless constructor to allow creation of test doubles.</summary>
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      protected CoverMetaClient() : base()
      {
      }
      /// <summary>Protected constructor to allow creation of configured clients.</summary>
      /// <param name="configuration">The client configuration.</param>
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      protected CoverMetaClient(ClientBaseConfiguration configuration) : base(configuration)
      {
      }

      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual global::GrpcCovers.AddCoverMetaResponse AddCoverMeta(global::GrpcCovers.AddCoverMetaRequest request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return AddCoverMeta(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual global::GrpcCovers.AddCoverMetaResponse AddCoverMeta(global::GrpcCovers.AddCoverMetaRequest request, grpc::CallOptions options)
      {
        return CallInvoker.BlockingUnaryCall(__Method_AddCoverMeta, null, options, request);
      }
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual grpc::AsyncUnaryCall<global::GrpcCovers.AddCoverMetaResponse> AddCoverMetaAsync(global::GrpcCovers.AddCoverMetaRequest request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return AddCoverMetaAsync(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual grpc::AsyncUnaryCall<global::GrpcCovers.AddCoverMetaResponse> AddCoverMetaAsync(global::GrpcCovers.AddCoverMetaRequest request, grpc::CallOptions options)
      {
        return CallInvoker.AsyncUnaryCall(__Method_AddCoverMeta, null, options, request);
      }
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual global::GrpcCovers.ReadCoverMetaResponse ReadCoverMeta(global::GrpcCovers.ReadCoverMetaRequest request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return ReadCoverMeta(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual global::GrpcCovers.ReadCoverMetaResponse ReadCoverMeta(global::GrpcCovers.ReadCoverMetaRequest request, grpc::CallOptions options)
      {
        return CallInvoker.BlockingUnaryCall(__Method_ReadCoverMeta, null, options, request);
      }
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual grpc::AsyncUnaryCall<global::GrpcCovers.ReadCoverMetaResponse> ReadCoverMetaAsync(global::GrpcCovers.ReadCoverMetaRequest request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return ReadCoverMetaAsync(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual grpc::AsyncUnaryCall<global::GrpcCovers.ReadCoverMetaResponse> ReadCoverMetaAsync(global::GrpcCovers.ReadCoverMetaRequest request, grpc::CallOptions options)
      {
        return CallInvoker.AsyncUnaryCall(__Method_ReadCoverMeta, null, options, request);
      }
      /// <summary>Creates a new instance of client from given <c>ClientBaseConfiguration</c>.</summary>
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      protected override CoverMetaClient NewInstance(ClientBaseConfiguration configuration)
      {
        return new CoverMetaClient(configuration);
      }
    }

    /// <summary>Creates service definition that can be registered with a server</summary>
    /// <param name="serviceImpl">An object implementing the server-side handling logic.</param>
    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    public static grpc::ServerServiceDefinition BindService(CoverMetaBase serviceImpl)
    {
      return grpc::ServerServiceDefinition.CreateBuilder()
          .AddMethod(__Method_AddCoverMeta, serviceImpl.AddCoverMeta)
          .AddMethod(__Method_ReadCoverMeta, serviceImpl.ReadCoverMeta).Build();
    }

    /// <summary>Register service method with a service binder with or without implementation. Useful when customizing the service binding logic.
    /// Note: this method is part of an experimental API that can change or be removed without any prior notice.</summary>
    /// <param name="serviceBinder">Service methods will be bound by calling <c>AddMethod</c> on this object.</param>
    /// <param name="serviceImpl">An object implementing the server-side handling logic.</param>
    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    public static void BindService(grpc::ServiceBinderBase serviceBinder, CoverMetaBase serviceImpl)
    {
      serviceBinder.AddMethod(__Method_AddCoverMeta, serviceImpl == null ? null : new grpc::UnaryServerMethod<global::GrpcCovers.AddCoverMetaRequest, global::GrpcCovers.AddCoverMetaResponse>(serviceImpl.AddCoverMeta));
      serviceBinder.AddMethod(__Method_ReadCoverMeta, serviceImpl == null ? null : new grpc::UnaryServerMethod<global::GrpcCovers.ReadCoverMetaRequest, global::GrpcCovers.ReadCoverMetaResponse>(serviceImpl.ReadCoverMeta));
    }

  }
}
#endregion
