// File: extension.js

const vscode = require('vscode');
const fs = require('fs');
const path = require('path');

/**
 * @param {vscode.ExtensionContext} context
 */
function activate(context) {
    let panel;
    let watcher;

    const command = vscode.commands.registerCommand('pbtInspector.showPanel', () => {
        if (panel) {
            panel.reveal();
            return;
        }

        panel = vscode.window.createWebviewPanel(
            'pbtInspector',
            'PBT Inspector',
            vscode.ViewColumn.One,
            {
                enableScripts: true,
                retainContextWhenHidden: true,
            }
        );

        const workspaceFolders = vscode.workspace.workspaceFolders;
        if (!workspaceFolders || workspaceFolders.length === 0) {
            vscode.window.showErrorMessage('No workspace folder open. Please open a folder.');
            return;
        }

        const logPath = path.join(workspaceFolders[0].uri.fsPath, 'pbt-inspector.ndjson');

        if (!fs.existsSync(logPath)) {
            vscode.window.showWarningMessage(`Log file not found: ${logPath}`);
            panel.webview.html = getWebviewContent(panel.webview, context.extensionUri);
            return;
        }

        try {
            watcher = fs.watch(logPath, () => {
                fs.readFile(logPath, 'utf8', (err, data) => {
                    if (err) return;
                    const lines = data.trim().split(/\r?\n/).filter(Boolean);
                    const entries = lines.map(line => {
                        try {
                            return JSON.parse(line);
                        } catch (e) {
                            return { error: 'Invalid JSON', raw: line };
                        }
                    });
                    panel.webview.postMessage({ command: 'updateLogs', entries: entries });
                });
            });
        } catch (error) {
            vscode.window.showErrorMessage(`Error watching log file: ${error.message}`);
        }

        panel.webview.html = getWebviewContent(panel.webview, context.extensionUri);

        panel.webview.onDidReceiveMessage(message => {
            if (message.command === 'requestLogs') {
                if (fs.existsSync(logPath)) {
                    fs.readFile(logPath, 'utf8', (err, data) => {
                        if (err) return;
                        const lines = data.trim().split(/\r?\n/).filter(Boolean);
                        const entries = lines.map(line => {
                            try {
                                return JSON.parse(line);
                            } catch (e) {
                                return { error: 'Invalid JSON', raw: line };
                            }
                        });
                        panel.webview.postMessage({ command: 'updateLogs', entries });
                    });
                }
            }
        });

        panel.onDidDispose(() => {
            panel = null;
            if (watcher) watcher.close();
        }, null, context.subscriptions);
    });

    context.subscriptions.push(command);
}

function getWebviewContent(webview, extensionUri) {
    const elmScript = webview.asWebviewUri(vscode.Uri.joinPath(extensionUri, 'media', 'elm.js'));
    const styleUri = webview.asWebviewUri(vscode.Uri.joinPath(extensionUri, 'media', 'style.css'));
    return `
    <!DOCTYPE html>
    <html lang="en">
    <head>
        <meta charset="UTF-8">
        <meta name="viewport" content="width=device-width, initial-scale=1.0">
        <title>PBT Inspector</title>
        <link href="${styleUri}" rel="stylesheet">
    </head>
    <body>
        <div id="elm-root"></div>
        <script src="${elmScript}"></script>
        <script>
            const vscode = acquireVsCodeApi();
            const app = Elm.Main.init({ node: document.getElementById('elm-root') });
            app.ports.requestLogs.subscribe(() => {
                vscode.postMessage({ command: 'requestLogs' });
            });
            window.addEventListener('message', event => {
            const message = event.data;
            if (message.command === 'updateLogs') {
                app.ports.receiveLogs.send(message.entries);
            }
        });
        </script>
    </body>
    </html>`;
}

function deactivate() { }

module.exports = {
    activate,
    deactivate
};
