import React from "react";
import { useAuth0 } from "@auth0/auth0-react";

const SignInButton = () => {
    const { isAuthenticated, loginWithRedirect } = useAuth0();

    return !isAuthenticated && (
        <button onClick={() => loginWithRedirect()}>
            Sign In
        </button>
    );
};

export default SignInButton;