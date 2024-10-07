using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentMigrator;

namespace Infrastructure.Migrations
{
    [Migration(20240110121100, TransactionBehavior.None)]
    public class InitSchema : Migration
    {
        public override void Up()
        {
            const string init_script = 
"""
CREATE TABLE IF NOT EXISTS track_meta
(
    track_id bigint NOT NULL GENERATED ALWAYS AS IDENTITY ( INCREMENT 1 START 1 MINVALUE 1 MAXVALUE 9223372036854775807 CACHE 1 ),
    album_id bigint,
    cover_art_id bigint,
    title character varying(50) COLLATE pg_catalog."default" NOT NULL,
    release_date date NOT NULL,
    CONSTRAINT release_meta_pkey PRIMARY KEY (track_id)
);


CREATE TABLE IF NOT EXISTS artist_meta
(
    artist_id bigint NOT NULL GENERATED ALWAYS AS IDENTITY ( INCREMENT 1 START 1 MINVALUE 1 MAXVALUE 9223372036854775807 CACHE 1 ),
    stage_name character varying(50) COLLATE pg_catalog."default" NOT NULL,
    real_name character varying(50) COLLATE pg_catalog."default",
    CONSTRAINT album_data_pkey PRIMARY KEY (artist_id)
);

CREATE TABLE IF NOT EXISTS album_meta
(
    album_id bigint NOT NULL GENERATED ALWAYS AS IDENTITY ( INCREMENT 1 START 1 MINVALUE 1 MAXVALUE 9223372036854775807 CACHE 1 ),
    title character varying(50) COLLATE pg_catalog."default" NOT NULL,
    release_date date NOT NULL,
    CONSTRAINT album_data_pkey1 PRIMARY KEY (album_id)
);

CREATE TABLE IF NOT EXISTS m2m_album_artist
(
    album_id bigint NOT NULL,
    artist_id bigint NOT NULL,
    CONSTRAINT m2m_album_artist_pkey PRIMARY KEY (artist_id, album_id),
    CONSTRAINT fk_album_id FOREIGN KEY (album_id)
        REFERENCES album_meta (album_id) MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE NO ACTION
        NOT VALID,
    CONSTRAINT fk_artist_id FOREIGN KEY (artist_id)
        REFERENCES artist_meta (artist_id) MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE NO ACTION
        NOT VALID
);

CREATE TABLE IF NOT EXISTS m2m_artist_track
(
    artist_id bigint NOT NULL,
    track_id bigint NOT NULL,
    CONSTRAINT m2m_artist_track_pkey PRIMARY KEY (artist_id, track_id),
    CONSTRAINT fk_artist_id FOREIGN KEY (artist_id)
        REFERENCES artist_meta (artist_id) MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE NO ACTION,
    CONSTRAINT fk_track_id FOREIGN KEY (track_id)
        REFERENCES track_meta (track_id) MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE NO ACTION
);

CREATE TYPE album_artist_link AS
(
	album_id bigint,
	artist_id bigint
);

CREATE TYPE track_artist_link AS
(
	track_id bigint,
	artist_id bigint
);

""";
            Execute.Sql(init_script);
        }

        public override void Down()
        {
            const string init_down_script =             
"""
drop type track_artist_link;
drop type album_artist_link;
DROP TABLE m2m_artist_track;
DROP TABLE m2m_album_artist;
DROP TABLE album_meta;
DROP TABLE artist_meta;
DROP TABLE track_meta;
""";
            Execute.Sql(init_down_script);
        }
    }
}