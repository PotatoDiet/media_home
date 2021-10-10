create table videos (
    id integer primary key,
    title text,
    year text,
    location text,
    community_rating number,
    poster text,
    current_watch_timestamp integer
);

create table genres (
    id integer primary key,
    name text unique
);

create table video_genres (
    id integer primary key,
    video_id integer,
    genre_id integer
);

create index genres_name on genres (name);
create index video_genres_video_id on video_genres (video_id);
create index video_genres_genre_id on video_genres (genre_id);
