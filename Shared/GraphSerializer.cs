#nullable enable
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace Shared
{
	public static class GraphSerializer
	{
		static public Graph? Deserialize(FileInfo file)
		{
			try
			{
				var deserializer = new XmlSerializer(typeof(List<EdgeEntity>));
				var edgeEntities = (List<EdgeEntity>)deserializer.Deserialize(File.OpenRead(file.FullName));

				var graph = new Graph(edgeEntities.SelectMany(x => new[] { x.VertexId1, x.VertexId2 }).Distinct().Select(id => new Vertex(id)).OrderBy(v => v.Id).ToList());
				edgeEntities.ForEach(e => graph.Connect(e.Id, e.VertexId1, e.VertexId2, e.Length));

				return graph;
			}
			catch (Exception e)
			{
				return null;
			}
		}

		public static void Serialize(Graph graph, FileInfo file)
		{

			var edges = graph.Edges.Select(x => new EdgeEntity()
			{
				Id = x.Id,
				VertexId1 = x.Vertex1.Id,
				VertexId2 = x.Vertex2.Id,
				Length = x.Length
			}).ToList();

			var serializer = new XmlSerializer(edges.GetType());
			var writer = new StreamWriter(file.FullName, false);
			serializer.Serialize(writer, edges);
			writer.Close();
		}

	}
}

