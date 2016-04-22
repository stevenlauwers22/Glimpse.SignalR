(function ($, pubsub, util, elements, data, renderEngine) {
    var context = { resultCount: 0, notice: null, isActive: false, contextRequestId: null, isSelected: false },
        generateResourceAddress = function () {
            var currentMetadata = data.currentMetadata();
            return util.uriTemplate(currentMetadata.resources.glimpse_signalr_invocations_data, { 'hash': currentMetadata.hash });
        },
        wireListeners = function () {
        },
        setup = function (args) {
            args.newData.data.signalRInvocations = { name: 'SignalR - Invocations', data: 'No invocations currently detected...', isPermanent: true };
            args.newData.metadata.plugins.signalRInvocations = {};
        },
        activate = function () {
            var options = elements.optionsHolder().html('<div class="glimpse-notice glimpse-disconnect"><div class="icon"></div><span>Disconnected...</span></div>');
            context.notice = util.connectionNotice(options.find('.glimpse-notice'));

            context.isSelected = true;

            listenStart();
        },
        deactivate = function () {
            elements.optionsHolder().html('');
            context.notice = null;

            listenStop();

            context.isSelected = false;
        },
        listenStart = function () {
            if (context.isSelected && !context.isActive) {
                context.isActive = true;
                fetch();
            }
        },
        listenStop = function () {
            if (context.isSelected && context.isActive) {
                context.isActive = false;
            }
        },
        fetch = function () {
            if (!context.isActive)
                return;

            //Poll for updated summary data
            context.notice.prePoll();
            $.ajax({
                url: generateResourceAddress(),
                type: 'GET',
                contentType: 'application/json',
                complete: function (jqXHR, textStatus) {
                    if (!context.isActive)
                        return;

                    context.notice.complete(textStatus);
                    setTimeout(fetch, 1000);
                },
                success: function (result) {
                    if (!context.isActive)
                        return;

                    layoutRender(result);
                }
            });
        },
        layoutRender = function (result) {
            if (context.resultCount == result.length)
                return;

            layoutBuildShell();
            layoutBuildContent(result);
        },
        layoutBuildShell = function () {
            var panel = elements.panel('signalRInvocations'),
                detailPanel = panel.find('table');

            if (detailPanel.length == 0) {
                var detailData = [['Hub', 'Method', 'Result', 'Arguments', 'Invoked on', 'Duration', 'Connection ID']];
                panel.html(renderEngine.build(detailData, null));
            }
        },
        layoutBuildContent = function (result) {
            var panel = elements.panel('signalRInvocations'),
                detailBody = panel.find('tbody'),
                html = '';

            for (var x = 0; x < result.length; x++) {
                var item = result[x];
                html += '<tr class="glimpse-row">';
                html += '<td>' + item.hub + '</td>';
                html += '<td>' + item.method + '</td>';

                if (item.result) {
                    html += '<td>' + item.result.type + ': ' + item.result.value + '</td>';
                } else {
                    html += '<td>None</td>';
                }

                if (item.arguments) {
                    html += '<td>';
                    for (var y = 0; y < item.arguments.length; y++) {
                        var arg = item.arguments[y];
                        html += arg.name + ' = ' + arg.type + ': ' + arg.value + '<br />';
                    }
                    html += '</td>';
                } else {
                    html += '<td>None</td>';
                }

                html += '<td>' + item.invokedOn + '</td>';
                html += '<td>' + item.duration + '</td>';
                html += '<td>' + item.connectionId + '</td>';
                html += '</tr>';
            }
            detailBody.html(html);
        };

    pubsub.subscribe('trigger.shell.subscriptions', wireListeners);
    pubsub.subscribe('action.panel.hiding.signalRInvocations', deactivate);
    pubsub.subscribe('action.panel.showing.signalRInvocations', activate);
    pubsub.subscribe('action.data.initial.changed', setup);
    pubsub.subscribe('action.shell.opening', listenStart);
    pubsub.subscribe('action.shell.closeing', listenStop);
    pubsub.subscribe('action.shell.minimizing', listenStop);

})(jQueryGlimpse, glimpse.pubsub, glimpse.util, glimpse.elements, glimpse.data, glimpse.render.engine);