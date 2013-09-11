using System;
using System.Collections.Generic;
using System.Linq;
using Nancy;
using Oak;

namespace __NAME__
{
    public class PeopleModule : NancyModule
    {
        dynamic db = new DynamicDb();

        public PeopleModule()
        {
            Get["/people"] = _ => NancyOak.Json(db.People().All());

            Get["/people/{id}"] = _ => NancyOak.Json(db.People().Single(_.id.Value));

            Post["/people/create"] = _ =>
            {
                db.People().Insert(this.Dto());

                return this.OK();
            };

            Post["/people/update/{id}"] = _ =>
            {
                db.People().Update(this.Dto(), _.id.Value);

                return this.OK();
            };

            Post["/delete/{id}"] = parameters =>
            {
                db.People().Delete(parameters.id.Value);

                return this.OK();
            };
        }
    }
}
