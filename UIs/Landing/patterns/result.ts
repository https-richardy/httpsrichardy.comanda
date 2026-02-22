import { Error } from "./error";

export class Result<TData = void> {
    public readonly isSuccess: boolean;
    public readonly isFailure: boolean;

    public readonly error: Error;
    public readonly value?: TData;

    private constructor(isSuccess: boolean, value?: TData, error: Error = Error.None) {
        this.isSuccess = isSuccess;
        this.isFailure = !isSuccess;
        this.error = error;
        this.value = value;
    }

    public static success<TData>(value: TData): Result<TData> {
        return new Result<TData>(true, value, Error.None);
    }

    public static failure<TData>(error: Error): Result<TData> {
        return new Result<TData>(false, undefined, error);
    }
}
