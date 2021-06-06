import React from "react";
import { useAuth0 } from "@auth0/auth0-react";

const SignInButton = () => {
    const { isLoading, isAuthenticated, loginWithRedirect } = useAuth0();

    if (isLoading)
        return null;

    return !isAuthenticated && (
        <button type="button" class="btn btn-primary" onClick={() => loginWithRedirect()}>
            Sign In
        </button>
    );
};

export default SignInButton;