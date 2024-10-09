using System;
using FluentMigrator;

namespace Infrastructure.Migrations
{
    [Migration(20240110121200, TransactionBehavior.None)]
    public class SeedData : Migration
    {
        public override void Up()
        {
            const string seed_script = 
"""
insert into artist_meta (artist_id, stage_name, real_name) 
values (1, 'Childish Gambino', 'Donald Glover')
	 , (2, 'Kendrick Lamar', 'Kendrick Duckworth Lamar')
	 , (3, 'Bruno Mars', 'Peter Gene Hernandez')
	 , (4, 'Bjork', 'Björk Guðmundsdóttir')
     , (5, 'Anderson .Paak', 'Brandon Paak Anderson');

insert into album_meta (album_id, title, release_date)
values (1, 'Bando Stone and The New World', '2024-07-19')
     , (2, 'Atavista', '2024-05-13')
     , (3, 'Mr. Morale & The Big Steppers', '2022-05-13')
     , (4, 'To Pimp A Butterfly', '2015-03-16')
     , (5, 'Vespertine', '2001-08-27')
     , (6, 'An Evening With Silk Sonic', '2021-11-11');

insert into m2m_album_artist (album_id, artist_id)
values (1, 1)
	 , (2, 1)
	 , (3, 2)
	 , (4, 2)
	 , (5, 4)
	 , (6, 3)
	 , (6, 5);

insert into track_meta (track_id, album_id, cover_art_id, title, release_date)
values (1, 1, 3, 'in the night', '2024-07-19')
	 , (2, 1, 3, 'yoshinoya', '2024-07-19')
	 , (3, 1, 3, 'talk my $h1t', '2024-07-19')
	 , (4, null, null, 'this is america', '2019-05-10')
	 , (5, 3, 1, 'mirror', '2022-05-13')
	 , (6, 4, null, 'alright', '2015-03-16')
	 , (7, 2, null, 'human sacrifice', '2024-05-13')
	 , (8, 5, null, 'pagan poetry', '2001-08-27')
	 , (9, 6, 2, 'blast off', '2021-11-11');

insert into m2m_artist_track (track_id, artist_id)
values (1, 1)
	 , (2, 1)
	 , (3, 1)
	 , (4, 1)
	 , (5, 2)
	 , (6, 2)
	 , (7, 2)
	 , (8, 4)
	 , (9, 3)
	 , (9, 5);
""";

            Execute.Sql(seed_script);
        }

        public override void Down()
        {
            const string seed_down_script = 
"""
delete from m2m_album_artist;
delete from m2m_artist_track;
delete from track_meta;
delete from album_meta;
delete from artist_meta;
"""; 
			Execute.Sql(seed_down_script);
        }
    }
}
