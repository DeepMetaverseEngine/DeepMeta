module.exports = app => {
  const { STRING, INTEGER, DATE, BOOLEAN } = app.Sequelize;

  const RecommendServer = app.model.define('recommend_server', {
    server_id: { type: INTEGER, primaryKey: true},
    period: INTEGER,
    created_at: DATE,
    updated_at: DATE,
  });

  RecommendServer.find = async function(server_id) {
    const one = await this.findOne({
      where: {
        server_id: server_id
      }
    });
    return one || {}
  }

  return RecommendServer;
};
