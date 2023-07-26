using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json.Serialization;
using System.Xml.Linq;

namespace ConsoleApp5
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            var client = new HttpClient();
            

            HttpResponseMessage response = client.GetAsync("https://swapi.dev/api/people").Result;  // Blocking call!  
            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("Request Message Information:- \n\n" + response.RequestMessage + "\n");
                Console.WriteLine("Response Message Header \n\n" + response.Content.Headers + "\n");
                // Get the response

                var customerJsonString =  response.Content.ReadAsStringAsync().Result;
                //Console.WriteLine("Your response data is: " + customerJsonString);

                dynamic stuff = JObject.Parse(customerJsonString);

                var result = stuff.results;
                var listResults = new List<people>();

                for(int i = 0 ; i < result.Count; i++)
                {
                    Console.WriteLine(result[i]);

                    var people = new people
                    {
                        name = result[i].name
                    };


                    people.films = new List<string>();
                    
                    for(int x =0  ; x < result[i].films.Count; x++)
                    {
                        people.films.Add(Convert.ToString(result[i].films[x]));
                    }


                    listResults.Add(people);

                   
                }

                //Console.ReadLine();

                //foreach(var res in result)
                //{
                //    var people = new people
                //    {
                //        name = res.name
                //    };

                //    people.films = new List<string>();
                //    foreach (var film in res.films)
                //    {
                //        people.films.Add(Convert.ToString(film));
                //    }


                //    listResults.Add(people);
                //    Console.ReadLine();
                //}

                // Build a uniqe list of films -- assuming always in order
                var uniqueFilms = new List<string>();

                foreach (var item in listResults)
                {
                    var resultFilm = "";
                    foreach(var film in item.films)
                    {
                        resultFilm += $"{film},";
                    }

                    uniqueFilms.Add(resultFilm);
                }

                var uf = uniqueFilms.ToList().Distinct();

                Console.WriteLine("RESULTS:");
                foreach(var u in uf)
                {
                    var resultOutput = "";

                    foreach(var p in listResults)
                    {
                        var thisPerson = "";

                        foreach (var film in p.films)
                        {
                            thisPerson += $"{film},";
                        }

                        if (thisPerson == u)
                        {
                            resultOutput += p.name;
                        }
                    }

                    Console.WriteLine(resultOutput);

                }

                    

                // Deserialise the data (include the Newtonsoft JSON Nuget package if you don't already have it)
                // var deserialized = JsonConvert.DeserializeObject<IEnumerable<Customer>>(custome‌​rJsonString);
                // Do something with it
                Console.ReadLine();
            }

            Console.ReadLine();
        }
    }
}
