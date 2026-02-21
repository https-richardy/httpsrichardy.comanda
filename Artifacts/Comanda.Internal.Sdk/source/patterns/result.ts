import { Error } from "../patterns/error";

export class Result<TData = void> {
    public readonly isSuccess: boolean;
    public readonly isFailure: boolean;

    public readonly error: Error;
    public readonly data?: TData;

    private constructor(isSuccess: boolean, data?: TData, error: Error = Error.None) {
        this.isSuccess = isSuccess;
        this.isFailure = !isSuccess;

        this.error = error;
        this.data = data;
    }

    public static success<TData>(data: TData): Result<TData> {
        return new Result<TData>(true, data, Error.None);
    }

    public static failure<TData>(error: Error): Result<TData> {
        return new Result<TData>(false, undefined, error);
    }
}
