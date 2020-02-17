using System;
using System.Collections.Generic;
using System.Linq;

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

    public class Example
    {
        #region Model

        class Item : IControlStringMatcher
        {
            public string addon;
            public string item;
            public string name;
            public string postfix;
            public string prefix;
            const string Space = " ";
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

            public bool IsVowel(char character) => 
                character == 'a' || character == 'A' ||
                character == 'e' || character == 'E' ||
                character == 'i' || character == 'I' ||
                character == 'o' || character == 'O' ||
                character == 'u' || character == 'U';

            public string Match(ControlString controlString)
            {
                if (!Matches(controlString))
                {
                    throw new ArgumentException("Argument cannot be matched by this matcher.", nameof(controlString));
                }

                return matcher.Match(controlString);
            }

            public bool Matches(ControlString controlString) => matcher.Matches(controlString);
        }

        class ItemFactory : IFactory<Item>
        {
            static readonly List<string> addons = new List<string>()
            {
                "Glorious",
                "Bloody",
                "Prolonged",
                "Bitter",
                "Wicked",
                "Furious"
            };

            static readonly List<string> items = new List<string>()
            {
                "Chainsaw",
                "Towel",
                "Ping-Pong Ball"
            };

            static readonly List<string> names = new List<string>()
            {
                "Trevor",
                "Jane",
                "HUMBOLDT THE UNDYING"
            };

            static readonly List<string> postfixes = new List<string>()
            {
                "Destruction",
                "Mutual Understanding",
                "Mediocrity"
            };

            static readonly List<string> prefixes = new List<string>()
            {
                "Ultimate",
                "Bloody",
                "Crooked"
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

        class Person : IControlStringMatcher
        {
            public string name;
            public string postfix;
            public string prefix;
            public string rank;

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

            public string FullName => 
                new Parser(Matchers).Parse("{Prefix[ ]}{Name}{[ ]Postfix}, {Rank}");

            public ControlStringMatcherCollection Matchers { get; set; }
            public Pronoun Pronouns { get; set; }

            public string Match(ControlString controlString)
            {
                if (!Matches(controlString))
                {
                    throw new ArgumentException("Argument cannot be matched by this matcher.", nameof(controlString));
                }

                return Matchers.Match(controlString);
            }

            public bool Matches(ControlString controlString) => Matchers.Matches(controlString);
        }

        private enum Gender
        {
            Male,
            Female,
            Neutral
        }

        class PersonFactory : IFactory<Person>
        {
            readonly List<string> maleNames = new List<string>()
            {
                "Aaron",
                "Abraham",
                "Adam"
            };

            readonly List<string> malePostfixes = new List<string>()
            {
                "Fairest of all",
                "The Hallowed Tyrant",
                "The Fat"
            };

            readonly List<string> malePrefixes = new List<string>()
            {
                "The Honourable",
                "The Barbarous",
                ""
            };

            readonly List<string> maleRanks = new List<string>()
            {
                "Earl",
                "Duke",
                "Lord"
            };

            readonly List<string> femaleNames = new List<string>()
            {
                "Alice",
                "Alana",
                "Barbara"
            };

            readonly List<string> femalePostfixes = new List<string>()
            {
                "Fairest of all",
                "The Iron Fist",
                "The Short"
            };

            readonly List<string> femalePrefixes = new List<string>()
            {
                "The Honourable",
                "The Barbarous",
                ""
            };

            readonly List<string> femaleRanks = new List<string>()
            {
                "Earl",
                "Duchess",
                "Lady"
            };

            public PersonFactory()
            {
                PronounFactory = new PronounFactory();
            }

            public IFactory<Pronoun, Gender> PronounFactory { get; set; }

            public Person Create()
            {
                var gender = Enum.GetValues(typeof(Gender)).Cast<Gender>().Random();

                switch(gender)
                {
                    case Gender.Female:
                        return new Person(PronounFactory.Create(gender))
                        {
                            postfix = femalePostfixes.Random(),
                            name = femaleNames.Random(),
                            rank = femaleRanks.Random(),
                            prefix = femalePrefixes.Random(),
                        };
                    case Gender.Male:
                        return new Person(PronounFactory.Create(gender))
                        {
                            postfix = malePostfixes.Random(),
                            name = maleNames.Random(),
                            rank = maleRanks.Random(),
                            prefix = malePrefixes.Random(),
                        };
                    default:
                        return new Person(PronounFactory.Create(gender))
                        {
                            postfix = malePostfixes.Random(),
                            name = maleNames.Random(),
                            rank = maleRanks.Random(),
                            prefix = malePrefixes.Random(),
                        };
                }
            }
        }

        class Pronoun : IControlStringMatcher
        {
            readonly IControlStringMatcher matcher;

            public Pronoun(string singular, string singularObject, string possessive, string reflexive, bool verbsEndInS)
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
                new ValueControlStringMatcher("Reflexive", ()=> Reflexive),
                new ContextControlStringMatcher("VerbEnding", new FuncControlStringMatcher(x=> verbsEndInS ? "s" :string.Empty,x=> true))
            });
            }

            public string Possessive { get; set; }
            public string Reflexive { get; set; }
            public string Singular { get; set; }
            public string SingularObject { get; set; }

            public string Match(ControlString controlString) => matcher.Match(controlString);

            public bool Matches(ControlString controlString) => matcher.Matches(controlString);
        }

        class PronounFactory : IFactory<Pronoun,Gender>
        {
            public Pronoun Create(Gender gender)
            {
                switch(gender)
                {
                    case (Gender.Male):
                        return MalePronoun();
                    case (Gender.Female):
                        return FemalePronoun();
                    default:
                        return NeutralPronoun();
                }
            } 

            Pronoun MalePronoun() => new Pronoun("he", "him", "his", "himself", true);

            Pronoun FemalePronoun() => new Pronoun("she", "her", "her", "herself", true);

            Pronoun NeutralPronoun() => new Pronoun("they", "them", "their", "themself", false);
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

            return parser.Parse("Before you stands {Person:FullName}.\n\n{Person:Pronoun:Singular} bring{Person:Pronoun:VerbEnding} you a gift, {Item:FullName}. This {Item:Item} is quite valuable.\n\nShould we kill {Person:Pronoun:SingularObject}?");
        }
    }
}