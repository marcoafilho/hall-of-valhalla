import React, { Component } from 'react';
import { NavMenu } from './NavMenu';

export class Layout extends Component {
    static displayName = Layout.name;

    render() {
        return (
            <>
                <header>
                    <NavMenu />
                </header>

                <main className="container p-3 bg-light">
                    {this.props.children}
                </main>
            </>
        );
    }
}
