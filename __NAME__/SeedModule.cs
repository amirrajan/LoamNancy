using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Oak;
using Nancy;
using Nancy.Hosting.Aspnet;

namespace __NAME__
{
    /*
    * Hi there.  This is where you define the schema for your database.  The nuget package rake-dot-net
    * has two commands that hook into this class.  One is 'rake reset' and the other is 'rake sample'.
    * The 'rake reset' command will drop all tables and regen your schema.  The 'rake sample' command
    * will drop all tables, regen your schema and insert sample data you've specified.  To get started
    * update the Scripts() method in the class below.
    */
    public class Schema
    {
        public IEnumerable<Func<dynamic>> Scripts()
        {
            yield return CreatePeopleTable;
        }

        public string CreatePeopleTable()
        {
            return Seed.CreateTable("People", Seed.Id(), new { Name = "nvarchar(255)" });
        }

        public void SampleEntries()
        {
            new { name = "Amir" }.InsertInto("People");
        }

        public Seed Seed { get; set; }

        public Schema(Seed seed) { Seed = seed; }
    }

    public class SeedModule : NancyModule
    {
         public Seed Seed { get; set; }

        public Schema Schema { get; set; }

        public SeedModule()
        {
            Seed = new Seed();

            Schema = new Schema(Seed);

            Post["seed/purgedb"] = _ =>
            {
                Seed.PurgeDb();

                return this.OK();
            };

            Post["seed/export"] = _ =>
            {
                var exportPath = new AspNetRootSourceProvider().GetRootPath();

                Seed.Export(exportPath, Schema.Scripts());

                return "Scripts executed to: " + exportPath;
            };

            Post["seed/all"] = _ =>
            {
                Schema.Scripts().ForEach<dynamic>(s => Seed.ExecuteNonQuery(s()));

                return this.OK();
            };

            Post["seed/sampleentries"] = _ =>
            {
                Schema.SampleEntries();

                return this.OK();
            };
        }
    }
}
