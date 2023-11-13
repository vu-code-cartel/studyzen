export type IdentifiableError = {
  errorCode: string;
};

export class ErrorCodes {
  public static readonly QuizGameNotFound = 'QuizGameNotFound';
  public static readonly QuizGameAlreadyStarted = 'QuizGameAlreadyStarted';
  public static readonly UsernameTaken = 'UsernameTaken';
}
