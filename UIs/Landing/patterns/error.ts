export class Error {
    public constructor(public code: string, public description: string) { }

    public static readonly None = new Error("", "");
    public static readonly Unknown = new Error("Unknown", "An unknown error has occurred.");

    public static from(code: string, description: string): Error {
        return new Error(code, description);
    }
}
