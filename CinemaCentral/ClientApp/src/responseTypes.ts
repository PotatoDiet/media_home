export type Library = {
    id: string;
    name: string;
    movies: Movie[];
    series: Series[];
}

export type Movie = {
    id: string;
    posterPath: string;
}

export type Series = {
    id: string;
    posterPath: string;
}

export type Media = {
    id: string;
    posterPath: string;
    mediaType: "Movie" | "Series";
}