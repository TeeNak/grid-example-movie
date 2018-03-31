package com.github.teenak.moviedemo.dto;

public class ErrorResponse extends Response {

    String message;

    public ErrorResponse() {
        super();
        setResult(false);
    }

    public ErrorResponse(String _message) {
        this();
        this.message = _message;
    }

    public String getMessage() {
        return message;
    }

    public void setMessage(String message) {
        this.message = message;
    }

}
