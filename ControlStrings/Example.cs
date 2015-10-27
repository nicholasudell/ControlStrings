using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ControlStrings.Example
{
    public static class IEnumerableExtensions
    {
        public static T Random<T>(this IEnumerable<T> collection)
        {
            var random = new Random();

            return collection.ElementAt(random.Next(0, collection.Count()));
        }
    }

    class Example
    {
        #region Model

        class PersonFactory : IFactory<Person>
        {
            public PersonFactory()
            {
                PronounFactory = new PronounFactory();
            }

            readonly List<string> malePrefixes = new List<string>()
            {
                "The Honourable",
                "The Barbarous",
                ""
            };
            readonly List<string> maleNames = new List<string>()
            {
                "Aaron",
                "Abraham",
                "Adam"
            };
            readonly List<string> maleRanks = new List<string>()
            {
                "Earl",
                "Duke",
                "Lord"
            };
            readonly List<string> malePostfixes = new List<string>()
            {
                "Fairest of all",
                "The Hallowed Tyrant",
                "The Fat"
            };

            public IFactory<Pronoun> PronounFactory { get; set; }

            public Person Create()
            {
                return new Person(PronounFactory.Create())
                {
                    postfix = malePostfixes.Random(),
                    name = maleNames.Random(),
                    rank = maleRanks.Random(),
                    prefix = malePrefixes.Random(),
                };
            }
        }


        class Pronoun : IControlStringMatcher
        {
            public string Singular { get; set; }

            public string Possessive { get; set; }

            public string SingularObject { get; set; }

            public string Reflexive { get; set; }

            public Pronoun(string singular, string singularObject, string possessive, string reflexive)
            {
                Singular = singular;
                SingularObject = singularObject;
                Possessive = possessive;
                Reflexive = reflexive;

                matcher = new ControlStringMatcherCollection(new List<IControlStringMatcher>()
            {
                new ValueControlStringMatcher("Singular", ()=> Singular),
                new ValueControlStringMatcher("SingularObject", ()=> SingularObject),
                new ValueControlStringMatcher("Possessive", ()=> Possessive),
                new ValueControlStringMatcher("Reflexive", ()=> Reflexive)
            });
            }

            readonly IControlStringMatcher matcher;

            public bool Matches(ControlString controlString)
            {
                return matcher.Matches(controlString);
            }

            public string Match(ControlString controlString)
            {
                return matcher.Match(controlString);
            }
        }

        class PronounFactory : IFactory<Pronoun>
        {
            public Pronoun Create()
            {
                return MalePronoun();
            }

            Pronoun MalePronoun()
            {
                return new Pronoun("he", "him", "his", "himself");
            }
        }

        class Person : IControlStringMatcher
        {

            public string prefix;
            public string name;
            public string rank;
            public string postfix;

            public string FullName
            {
                get
                {
                    return new Parser(Matchers).Parse("{Prefix[ ]}{Name}{[ ]Postfix}, {Rank}");
                }
            }

            public Pronoun Pronouns { get; set; }

            public ControlStringMatcherCollection Matchers { get; set; }

            public Person(Pronoun pronouns)
            {
                Pronouns = pronouns;

                Matchers = new ControlStringMatcherCollection(new List<IControlStringMatcher>()
                {
                    new ContextControlStringMatcher("Pronoun", Pronouns),
                    new ValueControlStringMatcher("Prefix", ()=> prefix),
                    new ValueControlStringMatcher("Postfix", ()=> postfix),
                    new ValueControlStringMatcher("Rank", ()=> rank),
                    new ValueControlStringMatcher("Name", ()=> name),
                    new ValueControlStringMatcher("FullName", ()=> FullName)
                });
            }

            public bool Matches(ControlString controlString)
            {
                return Matchers.Matches(controlString);
            }

            public string Match(ControlString controlString)
            {
                if (!Matches(controlString))
                {
                    throw new ArgumentException("Argument cannot be matched by this matcher.", "controlString");
                }

                return Matchers.Match(controlString);
            }
        }

        class Item : IControlStringMatcher
        {
            const string Space = " ";
            public string name;
            public string prefix;
            public string item;
            public string addon;
            public string postfix;
            readonly IControlStringMatcher matcher;

            public Item()
            {
                matcher = new ControlStringMatcherCollection(new List<IControlStringMatcher>()
                {
                    new ValueControlStringMatcher("Prefix", ()=> prefix),
                    new ValueControlStringMatcher("Item", ()=> item),
                    new ValueControlStringMatcher("Addon", ()=> addon),
                    new ValueControlStringMatcher("FullName", ()=> FullName),
                    new ValueControlStringMatcher("Postfix", ()=> postfix),
                    new ValueControlStringMatcher("Name", ()=> name)
                });
            }

            public string FullName
            {
                get
                {
                    string result = "";

                    if (!string.IsNullOrEmpty(name))
                    {
                        result += name + ", ";
                    }

                    if (!string.IsNullOrEmpty(prefix))
                    {
                        result += IsVowel(prefix.ToCharArray().First()) ? "an" : "a";
                        result += Space + prefix + Space + item;
                    }
                    else
                    {
                        result += IsVowel(item.ToCharArray().First()) ? "an" : "a";
                        result += Space + item;
                    }
                    if (!string.IsNullOrEmpty(postfix))
                    {
                        result += " of ";
                        if (!string.IsNullOrEmpty(addon))
                        {
                            result += Space + addon;
                        }

                        result += Space + postfix;
                    }

                    return result;
                }
            }

            public bool IsVowel(char character)
            {
                return
                character == 'a' || character == 'A' ||
                character == 'e' || character == 'E' ||
                character == 'i' || character == 'I' ||
                character == 'o' || character == 'O' ||
                character == 'u' || character == 'U';
            }

            public bool Matches(ControlString controlString)
            {
                return matcher.Matches(controlString);
            }

            public string Match(ControlString controlString)
            {
                if (!Matches(controlString))
                {
                    throw new ArgumentException("Argument cannot be matched by this matcher.", "controlString");
                }

                return matcher.Match(controlString);
            }
        }

        class ItemFactory : IFactory<Item>
        {
            static readonly List<string> prefixes = new List<string>()
            {
                "Ultimate",
                "Bloody",
                "Crooked"
            };

            static readonly List<string> items = new List<string>()
            {
                "Chainsaw",
                "Towel",
                "Ping-Pong Ball"
            };

            static readonly List<string> addons = new List<string>()
            {
                "Glorious",
                "Bloody",
                "Prolonged",
                "Bitter",
                "Wicked",
                "Furious"
            };

            static readonly List<string> postfixes = new List<string>()
            {
                "Destruction",
                "Mutual Understanding",
                "Mediocrity"
            };

            static readonly List<string> names = new List<string>()
            {
                "Trevor",
                "Jane",
                "HUMBOLDT THE UNDYING"
            };

            public Item Create()
            {
                bool generatePostfix = new Random().NextDouble() < 0.5f;

                return new Item()
                {
                    prefix = new Random().NextDouble() < 0.5f ? prefixes.Random() : null,
                    name = new Random().NextDouble() < 0.3f ? names.Random() : null,
                    item = items.Random(),
                    addon = generatePostfix && new Random().NextDouble() < 0.5f ? addons.Random() : null,
                    postfix = generatePostfix ? postfixes.Random() : null
                };
            }
        }

        #endregion Model

        public override string ToString()
        {
            var person = new PersonFactory().Create();
            var item = new ItemFactory().Create();

            var parser = new Parser(new ControlStringMatcherCollection(new List<IControlStringMatcher>()
            {
                new ContextControlStringMatcher("Person",  person),
                new ContextControlStringMatcher("Item", item)
            }));

            return parser.Parse("Before you stands {Person:FullName}.\n\n{Person:Pronoun:Singular} brings you a gift, {Item:FullName}. This {Item:Item} is quite valuable.\n\nShould we kill {Person:Pronoun:SingularObject}?");
        }
    }
}
